using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp;

public class Category
{
    private List<string> categories = new List<string>();

    public Category()
    {
        categories.Add("Beplantning");
        categories.Add("Græsplæne");
        categories.Add("Træfældning");
        categories.Add("Træterrasse");
        categories.Add("Udgravning");
        categories.Add("Bortkørsel af jord");
        categories.Add("Bortkørsel af jord");
        categories.Add("Bortkørsel af jord");
        categories.Add("Bortkørsel af jord");
        categories.Add("Bortkørsel af jord");
        categories.Add("Bortkørsel af jord");
        categories.Add("Bortkørsel af jord");
        categories.Add("Bortkørsel af jord");
        categories.Add("Bortkørsel af jord");
        categories.Add("Bortkørsel af jord");
    }

    public IEnumerable<string> GetCategories()
    {
        return categories;
    }
}
