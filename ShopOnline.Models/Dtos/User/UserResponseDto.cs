using System;
using System.Collections.Generic;
using System.Linq;

namespace ShopOnline.Models.Dtos.User
{
    public record UserResponseDto
    (
        int Id,
        string UserName,
        string FullName,
        string Email,
        int CartId,
        string JwtToken
    );
}
