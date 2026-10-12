## Steps to run the API
- Open solution in an IDE and click run. Or open a terminal, navigate to the directory and run the command `dotnet run`.
- Navigate to the swagger API documentation at `http://localhost:5212/swagger/index.html`
- Click on any of the endpoints to see the request and response formats
- To test the endpoints, you can use the "Try it out" button in swagger, this should give you a request body to complete, then click execute to send the request and see the response.
- Alternatively you can use a tool like Postman to send requests directly to the URL e.g. (`http://localhost:5212/api/Patients/{id}`).

## To do
- Add catches/middleware to handle exceptions and provide cleaner error messages on API responses
- Change return types in controllers to ApiModel for consistent naming/types across all API endpoints
- Add core functionality to set appointment to 'missed' if it's not set to 'attended' by the end of the appointment
- Convert duration and store as int for cleaner data