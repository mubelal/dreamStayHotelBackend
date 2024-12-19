using dreamStayHotel.Context;
using dreamStayHotel.DTO;
using dreamStayHotel.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace dreamStayHotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet("rooms")]
        public async Task<IActionResult> Get()
        {
            return Ok(await context.Rooms.ToListAsync());
        }

        [HttpGet("rooms/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await context.Rooms.FirstOrDefaultAsync(r => r.Id == id));
        }

        [HttpPost("rooms")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] Room room)
        {
            if (room is null) return BadRequest("Üres mező.");
            await context.Rooms.AddAsync(room);
            await context.SaveChangesAsync();
            return Ok("Szoba sikeresen létrejött.");
        }

        [HttpPut("rooms/{id}")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<IActionResult> Change([FromBody] Room room,int id)
        {
            if (room is null) return BadRequest("Töltse ki az üres mezőt.");
            var roomToChange = await context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            roomToChange.Description = room.Description;
            roomToChange.Img_Src = room.Img_Src;
            roomToChange.Name = room.Name;
            roomToChange.PricePerNight = room.PricePerNight;
            roomToChange.Rating = room.Rating;
            roomToChange.Extras = room.Extras;
            roomToChange.MaxAdults = room.MaxAdults;
            roomToChange.MaxChildren = room.MaxChildren;
            roomToChange.CheckInDate = room.CheckInDate;

            await context.SaveChangesAsync();
            return Ok("Sikeresen módosítottad a szobát.");
        }

        [HttpDelete("rooms/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var roomToDelete = await context.Rooms.FirstOrDefaultAsync(r =>r.Id == id);
            if (roomToDelete is null) return BadRequest("Általad választott szoba nem létezik");
            context.Rooms.Remove(roomToDelete);
            await context.SaveChangesAsync();
            return Ok("Szoba sikeresen törölve");

        }

        [HttpPost("rooms/sort-by")]
        
        public async Task<IActionResult> SortBy([FromBody] SortByDTO sortBy)
        {
            var rooms = await context.Rooms.Where(r => r.CheckInDate != null).Where(r => r.MaxAdults == sortBy.MaxAdults).Where(r => r.MaxChildren == sortBy.MaxChildren).ToListAsync();

            return Ok(rooms);
        }
    }
}
