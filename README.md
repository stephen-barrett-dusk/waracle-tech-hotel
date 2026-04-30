# waracle-tech-hotel
Technical Challenge - hotel booking

A simple hotel booking sample applicatiion as per the requirements of the Backend Developer Challenge.

# End Points

## Seeding

- `/seed/ResetBookings` will remove all bookings made.
- `/seed/ResetAllData` will remove all data and setup the hotels and rooms.

## Hotel

- `/Hotel/Search/<name>` search for a hotel my name
- `/Hotel` list all hotels
- `/Hotel/Availability/<hotelId>` find all available rooms in a hotel for given dates/guest counts

## Booking

- `/Booking/<reference>` returns details on a specific booking
- `/Booking/Book` Book a room.


