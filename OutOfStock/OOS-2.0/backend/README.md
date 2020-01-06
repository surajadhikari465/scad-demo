Out of Stock Mobile Backend
====================

This repository hosts the ASP.NET Core Web API consumed by the WFM Mobile hosted Out of Stock application. 

Introduction
------------
This API does not directly interact with the Out of Stock database, and instead serves only as a proxy to existing Out of Stock WCF services.

Development
-----------

### Requirements
- VS2019
- WFM Internal network access (VPN included)
- Docker *(Optional)*

### Build & Debug
1. Open the solution in Visual Studio
2. Select either "Docker" or "WFM.OutOfStock.API" from the profiles dropdown
3. Hit start.
4. Make sure you accept and confirm any certificate installation prompts you receive. 

### Accessing the API
If you selected Docker as the debug profile, you will be able to access the application with the following URLs:
- HTTP: `http://localhost:50692`
- HTTPS: `http://localhost:44349`

If you selected "WFM.Listomatic.API" as the debug profile, you should be able to access the application from the following URL:
- HTTP: `http://localhost:9191`

<!> NOTE: Due to some network quirks when testing with a device, the API can be set up to bind to `0.0.0.0`, making it accessible from outside the local machine, via your machine hostname, when running under the "WFM.Listomatic.API" profile. **To enable this, uncomment the commented line in Program.cs**.

This means that when running, the API is accessible over both of the following URLs:
- `http://{HOSTNAME}:9191/`

**<!> NOTE: This configuration will not work when running under the Docker profile.**

### Swagger UI
This project is Swagger UI enabled. The Swagger UI is accessible by appending `/swagger` to the end of one of the URLs provided above (e.g. `http://localhost:9191/swagger`).

---------------

Configuration
-------------

TBC.

---------------

Deployment
----------

Deployment
---
OOS Mobile is designed to run in AWS as a docker containerized application in an ECS cluster. Steps to deploy the application involve

1. Building the docker image
2. Pushing the docker image to the AWS ECR docker repository
3. Contacting Cloud Ops team to update the deployed task definition in the AWS ECS Cluster with the latest pushed image

Below is the Powershell script used to push the build and push the images. You will need AWS CLI installed and the AWS Access Key ID and AWS Secret Key. The CLI can be downloaded and the access keys can be obtained from the Cloud Ops team if needed.

Note: the following scripts assume that the full path to the Dockerfiles are under "C:\dev\SCAD". You may need to change this path based on your own location to the SCAD repo.

### DEV AWS Build

#### Backend
docker build -f "C:\dev\SCAD\OutOfStock\OOS-2.0\backend\src\WFM.OutOfStock.API\Dockerfile" -t 144478307378.dkr.ecr.us-east-1.amazonaws.com/oos-mobile-backend-ecr-repo 'C:\dev\SCAD\OutOfStock\OOS-2.0\backend\src\'
aws configure set aws_access_key_id **{AWS Access Key ID}**
aws configure set aws_secret_access_key **{AWS Access Secret Key}**
Invoke-Expression -Command (aws ecr get-login --no-include-email)
docker push 144478307378.dkr.ecr.us-east-1.amazonaws.com/oos-mobile-backend-ecr-repo

#### Frontend
docker build -f "C:\dev\SCAD\OutOfStock\OOS-2.0\frontend\Dockerfile" -t 144478307378.dkr.ecr.us-east-1.amazonaws.com/oos-mobile-frontend-ecr-repo 'C:\dev\SCAD\OutOfStock\OOS-2.0\frontend\'
aws configure set aws_access_key_id **{AWS Access Key ID}**
aws configure set aws_secret_access_key **{AWS Access Secret Key}**
Invoke-Expression -Command (aws ecr get-login --no-include-email)
docker push 144478307378.dkr.ecr.us-east-1.amazonaws.com/oos-mobile-frontend-ecr-repo