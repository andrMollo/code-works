namespace CodeWorksLibrary.Helpers
{
    public class PictureDispConverter : System.Windows.Forms.AxHost
    {
        public PictureDispConverter()
        : base("56174C86-1546-4778-8EE6-B6AC606875E7")
        {
        }

        public static System.Drawing.Image Convert(object objIDispImage)
        {
            System.Drawing.Image objPicture = default(System.Drawing.Image);
            objPicture = (System.Drawing.Image)System.Windows.Forms.AxHost.GetPictureFromIPicture(objIDispImage);
            return objPicture;
        }
    }
}
