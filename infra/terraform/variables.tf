variable "aws_region" { type = string, default = "ap-south-1" }
variable "name" { type = string, default = "ai-agent-builder" }
variable "image_uri" { type = string }
variable "openai_api_key" { type = string, sensitive = true }
variable "openai_model" { type = string, default = "gpt-4.1-mini" }
