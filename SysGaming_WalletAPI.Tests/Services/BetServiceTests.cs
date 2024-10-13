using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using SysGaming_WalletAPI.Models;
using SysGaming_WalletAPI.Services;
using SysGaming_WalletAPI.Controllers.DTO;
using SysGaming_WalletAPI.Exceptions;

public class BetServiceTests
{
    private readonly BetService _betService;
    private readonly AppDbContext _context;

    public BetServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new AppDbContext(options);
        var mockWebSocketManager = new Mock<WebSocketConnectionManager>();
        _betService = new BetService(_context, mockWebSocketManager.Object);

        // Adicionar dados iniciais
        SeedData();
    }

    private void SeedData()
    {
        var player = new Player
        {
            Id = 1,
            Wallet = new Wallet { PlayerId = 1, Balance = 100 }
        };

        _context.Players.Add(player);
        _context.SaveChanges();
    }

    [Fact]
    public async Task CreateBet_ShouldDeductBalance_WhenBetIsPlaced()
    {
        // Arrange
        var betDTO = new BetDTO { PlayerId = 1, Value = 50 };

        // Act
        var bet = await _betService.CreateBet(betDTO);

        // Assert
        var player = await _context.Players.Include(p => p.Wallet).FirstOrDefaultAsync(p => p.Id == 1);
        Assert.NotNull(bet);
        if(bet.Status == BetStatus.WON){
            Assert.Equal(100, player.Wallet.Balance); 
        }else{
            Assert.Equal(0, player.Wallet.Balance); 
        }
    }

    [Fact]
    public async Task CreateBet_ShouldThrowException_WhenInsufficientBalance()
    {
        // Arrange
        var betDTO = new BetDTO { PlayerId = 1, Value = 200 };

        // Act & Assert
        await Assert.ThrowsAsync<InsuficientBalanceException>(() => _betService.CreateBet(betDTO));
    }

    [Fact]
    public async Task CreateBet_ShouldThrowException_WhenPlayerDoesNotExist()
    {
        // Arrange
        var betDTO = new BetDTO { PlayerId = 99, Value = 50 };

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _betService.CreateBet(betDTO));
    }
    
    [Fact]
    public async Task HandleLostBet_ShouldApplyBonus_AfterFiveLostBets()
    {
        for (int i = 0; i < 5; i++)
        {
            var bet = new Bet
            {
                PlayerId = 1,
                Value = 10,
                Status = BetStatus.LOST,
                DateTime = DateTime.UtcNow.AddMinutes(-i) 
            };
            _context.Bets.Add(bet);
        }
        await _context.SaveChangesAsync();

        var player = await _context.Players.Include(p => p.Wallet).FirstOrDefaultAsync(p => p.Id == 1);
        var initialBalance = player.Wallet.Balance;

        var bonus = await _betService.HandleLostBetAsync(1);

        Assert.Equal(5, await _context.Bets.CountAsync()); 
        Assert.True(bonus > 0);
        Assert.Equal(5, await _context.Transactions.CountAsync()); 

        var expectedBonus = 5 * 10 * 0.10m; 
        Assert.Equal(expectedBonus, bonus); 

        var updatedPlayer = await _context.Players.Include(p => p.Wallet).FirstOrDefaultAsync(p => p.Id == 1);
        Assert.Equal(initialBalance + expectedBonus, updatedPlayer.Wallet.Balance);
    }

}