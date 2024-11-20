using ContractMonthlyClaimSystem.Data;
using ContractMonthlyClaimSystem.Hubs;
using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

public class ClaimController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<ClaimsHub> _hubContext;

    public ClaimController(ApplicationDbContext context, IHubContext<ClaimsHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    // Action to view all Pending claims
    public IActionResult Pending()
    {
        var pendingClaims = _context.Claims.Where(c => c.Status == "Pending").ToList();
        return View(pendingClaims);  // Pass the list of pending claims to the Pending view
    }

    public async Task<IActionResult> Approve(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim == null) return NotFound();

        claim.Status = "Approved";
        await _context.SaveChangesAsync();

        // Send real-time update to all connected clients
        await _hubContext.Clients.All.SendAsync("ClaimStatusUpdated", claim.Id, "Approved");

        return RedirectToAction(nameof(Pending)); // Redirect to the Pending view after approval
    }

    public async Task<IActionResult> Reject(int id)
    {
        var claim = await _context.Claims.FindAsync(id);
        if (claim == null) return NotFound();

        claim.Status = "Rejected";
        await _context.SaveChangesAsync();

        // Send real-time update to all connected clients
        await _hubContext.Clients.All.SendAsync("ClaimStatusUpdated", claim.Id, "Rejected");

        return RedirectToAction(nameof(Pending)); // Redirect to the Pending view after rejection
    }
}
