using Buttler.Application.DTO;
using Buttler.Domain.Data;
using CoreHtmlToImage;
using System.Text;
using TuesPechkin;

namespace Buttler_3._0.Services
{
    public class PdfService : IPDFservice
    {
        private readonly ButtlerContext _context;

        public PdfService(ButtlerContext context)
        {
            _context = context;
        }

        public byte[] GenerateRecipte(ReceiptDto receipt)
        {
            var sb = new StringBuilder();

            // html start
            sb.Append("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n");

            // Header
            sb.Append("<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css\" rel=\"stylesheet\"\r\n        integrity=\"sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM\" crossorigin=\"anonymous\">\r\n    <title>Bill</title>\r\n\r\n</head>");

            // Main body
            sb.Append("<body style=\"font-family: 'JetBrains Mono';\" class=\"px-4 py-3\">\r\n");
            sb.Append($"<h1 class=\"d-flex justify-content-center align-items-center\">Bill Receipt #00{receipt.OrderId}</h1>\r\n");
            sb.Append($"<p>Customer name: {receipt.CustomerName}</p>\r\n");
            sb.Append($"<p>Phone number: {receipt.CustomerPhoneNumber}</p>\r\n");
            sb.Append($"<p>Total bill: ${receipt.Bill}</p>\r\n");
            sb.Append($"<p>Date of bill: {receipt.DateOfOrder}</p>\r\n");
            sb.Append("<hr>\r\n");
            sb.Append("<table class=\"table table-responsive table-striped\">\r\n");
            sb.Append(" <thead class=\"bg-dark text-white fw-semibold\">\r\n            <tr>\r\n                <th>#</th>\r\n                <th>Food Items</th>\r\n                <th>Qty</th>\r\n                <th>Price</th>\r\n            </tr>\r\n        </thead>");
            sb.Append("<tbody>\r\n");
            if (receipt.FoodId != null)
            {
                foreach (var item in receipt.FoodId)
                {
                    sb.Append("<tr>\r\n");
                    sb.Append($"<th>#{item.FoodItemId}</th>\r\n");

                    sb.Append($"<th>{_context.Foods.FirstOrDefault(r => r.FoodsId == item.FoodItemId)?.Title}</th>\r\n");
                    sb.Append($"<th>{receipt.Qty}</th>\r\n");
                    sb.Append($"<th>${_context.Foods.FirstOrDefault(r => r.FoodsId == item.FoodItemId)?.Price}</th>\r\n");
                    sb.Append("</tr>\r\n");
                }
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            sb.Append("<script src=\"https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js\"\r\n        integrity=\"sha384-I7E8VVD/ismYTF4hNIPjVp/Zjvgyol6VFvRkX/vR+Vc4jQkC+hVqc2pM8ODewa9r\"\r\n        crossorigin=\"anonymous\"></script>\r\n    <script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.min.js\"\r\n        integrity=\"sha384-fbbOQedDUMZZ5KreZpsbe1LCZPVmfTnH7ois6mU1QK+m14rQ1l2bGBq41eYeM/fS\"\r\n        crossorigin=\"anonymous\"></script>");

            sb.Append("</body>\r\n");

            sb.Append("</html>\r\n");

            var converter = new StandardConverter(new PdfToolset(new Win64EmbeddedDeployment(new TempFolderDeployment())));
            var document = new HtmlToPdfDocument()
            {
                Objects =
                {
                    new ObjectSettings{HtmlText=sb.ToString() }
                },
            };

            byte[] result = converter.Convert(document);
            return result;
        }
    }

    public interface IPDFservice
    {
        byte[] GenerateRecipte(ReceiptDto receipt);
    }
}
