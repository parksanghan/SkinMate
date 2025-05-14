using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Data
{
    //유저간 채팅 메시지 
    public class UserChatMessage
    {
        public string? UserId { get; set; } 
        public string? Message { get; set; }
        public string? ImageUrl {  get; set; }  
    }
}
