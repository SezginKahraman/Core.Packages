﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;

public class InternalServerErrorProblemDetails : ProblemDetails
{
    public InternalServerErrorProblemDetails(string details)
    {
        Title = "Internal server error.";
        Detail = details;
        Status = StatusCodes.Status500InternalServerError;
        Type = "https://example.com/probs/business";
    }
}