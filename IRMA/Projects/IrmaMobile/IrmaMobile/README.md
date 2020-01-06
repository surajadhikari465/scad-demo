IRMA Mobile
===

This project hosts the ASP.NET Core backend and REACT frontend IRMA Mobile application. The frontend is in the ClientApp folder under the project.

Introduction
---
IRMA Mobile is an application under the WFM Mobile project that allows users to Shrink items, create Transfer orders, and Receive orders in IRMA from the Honeywell CT60 devices.

The IRMA Mobile backend exposes an API for the frontend to deliver data to the frontend. It retrieves that data from the legacy IRMA Service which is a WCF service hosted in IIS on on-premise servers.

Development
---

### Requirements
- VS2019
- WFM Internal network access (VPN included)
- Docker *(Optional)*
- NPM
- Access to SCMPA team's code repository

## Build and Run

#### Backend
1. Open the solution in Visual Studio
2. Select either "Docker" or "IrmaMobile" from the profiles dropdown
3. Hit start
4. Make sure you accept and confirm any certificate installation prompts you receive
5. The backend startup URLs are hosted are under the launchSettings.json file. In order to allow local testing on the device we've set the URL to http://0.0.0.0:5000 but to hit the API without local testing you'll need to go to http://localhost:5000

#### Frontend
1. Navigate to the ClientApp folder through Command Line, Powershell, or Terminal of your choice
2. Run "npm install" from terminal to install npm packages
3. Run "npm start" from terminal to start the frontend

Note: the frontend relies on the WFM UI library written by the SCMPA team. The WFM UI npm library location exists under the .npmrc file.

Configuration
---

#### Connection to the Legacy IRMA Service
WCF connections to the Legacy IRMA Service are stored in the appSettings.json under IRMAMobile.ServiceUris. Each IRMA region is given a specific service URI. Change these URIs to test against different instances of the legacy services.

#### Frontend connection to the backend
The URL for the frontend to use to connect to the backend is stored in the ClientApp/src/config.ts file. It uses the REACT_APP_BASE_URL which is an environment set variable. That variable is set by the ClientApp/.env file. Different .env will set the URL based on the environment you are in.

To add new environments for the frontend 
1. Add a new .env.{name of environment} file under CLientApp.
2. Update package.json with a script entry for start and build for your new environment. For example:

    "start:{name of environment}": "env-cmd -f .env.{name of environment} react-scripts start"

    "build:{name of environment}": "env-cmd -f .env.{name of environment} react-scripts build"

3. Run "npm run start:{name of environment}" to start the frontend with that environment
4. Run "npm run build:{name of environment}" to build a release of the frontend for that environment

Deployment
---
IRMA Mobile is designed to run in AWS as a docker containerized application in an ECS cluster. Steps to deploy the application involve

1. Building the docker image
2. Pushing the docker image to the AWS ECR docker repository
3. Contacting Cloud Ops team to update the deployed task definition in the AWS ECS Cluster with the latest pushed image

Below is the Powershell script used to push the build and push the images. You will need AWS CLI installed and the AWS Access Key ID and AWS Secret Key. The CLI can be downloaded and the access keys can be obtained from the Cloud Ops team if needed.

Note: the following scripts assume that the full path to the Dockerfiles are under "C:\dev\SCAD". You may need to change this path based on your own location to the SCAD repo.

### DEV AWS Build

#### Backend
docker build -f "C:\dev\SCAD\IRMA\Projects\IrmaMobile\IrmaMobile\Dockerfile" -t 144478307378.dkr.ecr.us-east-1.amazonaws.com/oos-irma-backend-ecr-repo 'C:\dev\SCAD\IRMA\Projects\IrmaMobile'

aws configure set aws_access_key_id **{AWS Access Key ID}**

aws configure set aws_secret_access_key **{AWS Access Secret Key}**

Invoke-Expression -Command (aws ecr get-login --no-include-email)

docker push 144478307378.dkr.ecr.us-east-1.amazonaws.com/oos-irma-backend-ecr-repo

#### Frontend
docker build -f "C:\dev\SCAD\IRMA\Projects\IrmaMobile\IrmaMobile\ClientApp\Dockerfile" -t 144478307378.dkr.ecr.us-east-1.amazonaws.com/oos-irma-frontend-ecr-repo 'C:\dev\SCAD\IRMA\Projects\IrmaMobile\IrmaMobile\ClientApp'

aws configure set aws_access_key_id **{AWS Access Key ID}**

aws configure set aws_secret_access_key **{AWS Access Secret Key}**

Invoke-Expression -Command (aws ecr get-login --no-include-email)

docker push 144478307378.dkr.ecr.us-east-1.amazonaws.com/oos-irma-frontend-ecr-repo