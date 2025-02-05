using dreamStayHotel.Context;
using dreamStayHotel.DTO;
using dreamStayHotel.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dreamStayHotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ControllerBase
    {
        async Task<Booking> CreateBookingAsync(CreateBookingDTO dto, string userId)
        {
            // Validate the roomId exists
            var room = await context.Rooms.FindAsync(dto.roomId);
            if (room == null)
            {
                throw new ArgumentException("Room not found.");
            }

            // Create a new Booking entity from the DTO
            var booking = new Booking
            {
                userId = userId,  // Set the userId directly
                CheckInDate = dto.CheckInDate,
                CheckOutDate = dto.CheckOutDate,
                Room = room       // Set the found room
            };


            context.Entry(room).State = EntityState.Unchanged;
            // Add the booking to the context and save changes
            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();

            return booking;
        }

        [HttpPost("booking")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO dto)
        {
            // Get the userId from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found.");
            }

            try
            {
                var booking = await CreateBookingAsync(dto, userId);
                return Ok(booking); // Return the created booking
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Return error if room is not found or invalid roomId
            }
        }

        [HttpDelete("booking/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bookingToDelete = await context.Bookings.FirstOrDefaultAsync(r => r.id == id);
            if (bookingToDelete is null) return BadRequest("Általad választott foglalás nem létezik");
            context.Bookings.Remove(bookingToDelete);
            await context.SaveChangesAsync();
            return Ok("Foglalás sikeresen törölve");
        }

        [HttpPut("booking/{id}/verify")]
        public async Task<IActionResult> VerifyBooking(int id)
        {
            var booking = await context.Bookings.FirstOrDefaultAsync(r => r.id == id);
            if (booking is null) return BadRequest("Általad választott foglalás nem létezik");
            booking.Status = "Elfogadva";
            await context.SaveChangesAsync();
            return Ok("Foglalás sikeresen elfogadva");
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found.");
            }

            return Ok(await context.Bookings.Where(b => b.userId == userId).Include(b => b.Room).ToListAsync());
        }
    }
}
