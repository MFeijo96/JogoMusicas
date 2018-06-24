using System;
using System.Collections.Generic;
using System.Text;

namespace JogoDasMusicas.ViewModel
{
    public class GameViewModel
    {
        var values = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(content);
    }
}
