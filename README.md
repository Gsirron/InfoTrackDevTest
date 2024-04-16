# InfoTrack BookingApi

### Summary
- Built using .NET 8
- Uses EF Core In-Memory Database Provider
- Tests using Xunit


### Link to Repository
[Link to Github Repository](https://github.com/Gsirron/InfoTrackDevTest/)

### Instructions
- In the 




### Api Endpoints

```Html
https://localhost:7038/api/booking/create

http://localhost:5242/api/booking/create
```




#### Create New booking

<details>
 <summary><code>POST</code> <code><b>/</b></code> <code>api/booking/create</code></summary>

##### Parameters

> | name      |  type     | data type               | description                                                           |
> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|
> | name      |  required | string                     | Name of the Booker|
> | bookingTime      |  required | string                    | Time in 24 hour time format HH:mm |

##### Responses

> | http code     | content-type                      | response                                                            |
> |---------------|-----------------------------------|---------------------------------------------------------------------|
> | `200`         | `application/json`                  | `bookingId : {GUID}`                                |
> | `400`         | `application/json`                | `{"status":"400","errors":{"Error Message"}}`                            |
> | `409`         | `application/json`                | `"error": "Settlements are full for this time of {time}`                                                               |

</details>

## Technical requirements

- Assume that all bookings are for the same day (do not worry about handling dates)
- InfoTrack's hours of business are 9am-5pm, all bookings must complete by 5pm (latest booking is 4:00pm)
- Bookings are for 1 hour (booking at 9:00am means the spot is held from 9:00am to 9:59am)
- InfoTrack accepts up to 4 simultaneous settlements
- API needs to accept POST requests of the following format:
`{
"bookingTime": "09:30",
"name":"John Smith"
}`
- Successful bookings should respond with an OK status and a booking Id in GUID form
`{
"bookingId": "d90f8c55-90a5-4537-a99d-c68242a6012b"
}`
- Requests for out of hours times should return a Bad Request status
- Requests with invalid data should return a Bad Request status
- Requests when all settlements at a booking time are reserved should return a Conflict status
- The name property should be a non-empty string
- The bookingTime property should be a 24-hour time (00:00 - 23:59)
- Bookings can be stored in-memory, it is fine for them to be forgotten when the application is restarted

## Deliverable

- A link to your source control system containing your API code
- Instructions on the endpoint to contact when we run your solution
- Any additional details you feel are necessary to execute or understand your solution
- Target 1-2 hours on this solution

## License

[MIT](https://choosealicense.com/licenses/mit/)