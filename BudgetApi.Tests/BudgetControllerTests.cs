using BudgetApi.Controllers;
using BudgetApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Xunit;

public class BudgetControllerTests
{
    [Fact]
    public void Calculate_WithoutInsurance_Returns500()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 100m,
            Days = 5,
            IncludeInsurance = false
        };

        var result = controller.Calculate(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        var response = JsonSerializer.SerializeToElement(ok.Value);
        decimal total = response.GetProperty("TotalAmount").GetDecimal();
        Assert.Equal(500m, total);
    }

    [Fact]
    public void Calculate_WithInsurance_Returns525()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 100m,
            Days = 5,
            IncludeInsurance = true
        };

        var result = controller.Calculate(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        var response = JsonSerializer.SerializeToElement(ok.Value);
        decimal total = response.GetProperty("TotalAmount").GetDecimal();
        Assert.Equal(525m, total);
    }

    [Fact]
    public void Calculate_NegativeDays_ReturnsBadRequest()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 100m,
            Days = -1,
            IncludeInsurance = false
        };

        var result = controller.Calculate(request);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }

    [Fact]
    public void Calculate_ZeroDailyCost_ReturnsZero()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 0m,
            Days = 5,
            IncludeInsurance = false
        };

        var result = controller.Calculate(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        var response = JsonSerializer.SerializeToElement(ok.Value);
        decimal total = response.GetProperty("TotalAmount").GetDecimal();
        Assert.Equal(0m, total);
    }

    [Fact]
    public void Calculate_DecimalDailyCost_Returns301Point50()
    {
        var controller = new BudgetController();
        var request = new BudgetRequest
        {
            DailyCost = 100.50m,
            Days = 3,
            IncludeInsurance = false
        };

        var result = controller.Calculate(request);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
        var response = JsonSerializer.SerializeToElement(ok.Value);
        decimal total = response.GetProperty("TotalAmount").GetDecimal();
        Assert.Equal(301.50m, total);
    }
}
