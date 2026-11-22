terraform {
  required_version = ">= 1.6.0"
  required_providers { aws = { source = "hashicorp/aws", version = "~> 5.0" } }
}
provider "aws" { region = var.aws_region }
resource "aws_secretsmanager_secret" "openai" { name = "${var.name}/openai-api-key" }
resource "aws_secretsmanager_secret_version" "openai" {
  secret_id = aws_secretsmanager_secret.openai.id
  secret_string = var.openai_api_key
}
resource "aws_apprunner_auto_scaling_configuration_version" "app" {
  auto_scaling_configuration_name = "${var.name}-scaling"
  max_concurrency = 20
  max_size = 2
  min_size = 1
}
resource "aws_apprunner_service" "app" {
  service_name = var.name
  auto_scaling_configuration_arn = aws_apprunner_auto_scaling_configuration_version.app.arn
  source_configuration {
    auto_deployments_enabled = false
    image_repository {
      image_identifier = var.image_uri
      image_repository_type = "ECR"
      image_configuration {
        port = "8080"
        runtime_environment_variables = { "OpenAI__Model" = var.openai_model }
        runtime_environment_secrets = { "OpenAI__ApiKey" = aws_secretsmanager_secret.openai.arn }
      }
    }
    authentication_configuration { access_role_arn = aws_iam_role.apprunner_ecr.arn }
  }
  instance_configuration { cpu = "1 vCPU" memory = "2 GB" }
  depends_on = [aws_secretsmanager_secret_version.openai]
}
resource "aws_iam_role" "apprunner_ecr" {
  name = "${var.name}-apprunner-ecr"
  assume_role_policy = jsonencode({Version="2012-10-17",Statement=[{Effect="Allow",Principal={Service="build.apprunner.amazonaws.com"},Action="sts:AssumeRole"}]})
}
resource "aws_iam_role_policy_attachment" "ecr" {
  role = aws_iam_role.apprunner_ecr.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSAppRunnerServicePolicyForECRAccess"
}
output "service_url" { value = aws_apprunner_service.app.service_url }
