namespace BLL.Helpers
{
    public class Paging
    {
        public int SkipItem(int page, int limit)
        {
            int result;

            if (page < 1 || limit < 1)
            {
                return -1;
            }
            result = (page - 1) * limit;
            return result;
        }

        public int GetPageTotal(int items, int pageitems)
        {
            if (pageitems < 1)
            {
                return -1;
            }

            double numOfPage = items / pageitems;
            int itemLeft = items % pageitems;

            if (itemLeft != 0)
            {
                numOfPage++;
            }
            return (int)numOfPage;
        }
    }
}
