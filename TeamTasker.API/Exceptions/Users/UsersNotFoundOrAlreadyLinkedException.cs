﻿using BuildingBlocks.Exceptions;

namespace TeamTasker.API.Exceptions.Users
{
    public class UsersNotFoundOrAlreadyLinkedException : BadRequestException
    {
        public UsersNotFoundOrAlreadyLinkedException(string message = "Users not found or already linked to task", string? details = null) : base(message, details) { }
    }
}
