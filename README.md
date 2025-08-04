# asre-test
A test project for ASRE

*NOTE: To conform to requirements of assessment all projects run under .NET 6 which is out of support. The better choice would be to run applications under LTS version of .NET*

The solution contains of three projects:
- ASRE.DataLayer
- ASRE.PatientApi
- ASRE.DataGenerator

The solution follows simple layer-based architecture with DAL, SERVICE and API layers. DAL is represented by ASRE.DataLayer project where Entity Framework context and entities are configured, while SERVICE and API layers are in ASRE.PatientApi project. This is sufficient for this test project

The ASRE.DataGenerator is a console application that generates fake patients' data using POST /patients endpoint. It is configured to make calls to API which is run under Docker on port 8080. The API base URI can be changed in appsettings.json file

To run the API in Docker container navigate to .\asre-test\ASRE and run `docker-compose up --build` command.
Make sure that you have Docker installed on the machine!
Navigate to http://localhost:8080/swagger/index.html in your browser to see Swagger page

Postman collection is available in the root of the repository as JSON file (ASRE Patients.postman_collection.json)