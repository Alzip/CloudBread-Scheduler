# CloudBread-Scheduler
This porject is scheduler batch service project of CloudBread. 

##Install guide
- Fork this project to your repository
- Clone or download the project
- Open it in Visual Studio 2015 
- Change app.config in your Visual Studio 2015 (or change it on Azure Portal Application Setting) 
- Build and publish "Publish as Azure Web Jon" to your CloudBread Admin web app

###Default batch
- DAU(Daily)
- HAU(Hourly)
- DARPU(Daily)

###Configuration
Open app.config and fill out email, slack, storage account and database etc.

###Note. 
There're couple ways to deploy webjob to Azure app service. But, Visual Studio should be the best one at this time.
(wrote in 2016-03-03)
