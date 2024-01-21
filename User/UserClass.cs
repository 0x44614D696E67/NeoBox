using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeoBox.User;
public class UserClass
{
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public bool IsEnable { get; set; }
}
