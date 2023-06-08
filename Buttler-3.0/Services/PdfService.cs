using Buttler.Application.DTO;
using Buttler.Domain.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.Text;

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
            StringBuilder stringBuilder = new();

            // Append the HTML code
            stringBuilder.Append("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n");

            //head
            stringBuilder.Append("<head>\n");
            stringBuilder.Append("<title>Billing</title>\n");
            stringBuilder.Append("<meta charset=\"UTF-8\"/>\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"/>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>\r\n    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css\" rel=\"stylesheet\"\r\n        integrity=\"sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM\" crossorigin=\"anonymous\"/>");
            stringBuilder.Append("</head>\n");

            // body
            stringBuilder.Append("<body style=\"font-family: 'JetBrains Mono';\" class=\"px-4 py-3\">\r\n");
            stringBuilder.Append($"<h1 class=\"d-flex justify-content-center align-items-center\">Bill Receipt #00{receipt.OrderId}</h1>\r\n");
            stringBuilder.Append($"<p>Customer name: {receipt.CustomerName}</p>\r\n");
            stringBuilder.Append($"    <p>Phone number: {receipt.CustomerPhoneNumber}</p>\r\n");
            stringBuilder.Append($"    <p>Total bill: ${receipt.Bill}</p>\r\n");
            stringBuilder.Append($"    <p>Date of bill: ${receipt.DateOfOrder}</p>\r\n");
            stringBuilder.Append("    <hr/>\r\n");

            stringBuilder.Append("    <table class=\"table table-responsive table-striped w-100 text-center\">\r\n");
            stringBuilder.Append("<thead>\n");
            stringBuilder.Append("<tr>\n");
            stringBuilder.Append("<th>Sr.No.</th>\n");
            stringBuilder.Append("<th>Food Item</th>\n");
            stringBuilder.Append("<th>QTY</th>\n");
            stringBuilder.Append("<th>Price</th>\n");
            stringBuilder.Append("<th>Total</th>\n");
            stringBuilder.Append("</tr>\n");
            stringBuilder.Append("</thead>\n");
            stringBuilder.Append("<tbody>\n");
            // Append billing data dynamically
            if (receipt.FoodId != null)
            {
                foreach (var item in receipt.FoodId)
                {
                    stringBuilder.Append("<tr>\n");
                    stringBuilder.Append($"<td>#{item.FoodItemId + 1}</td>\n");
                    stringBuilder.Append($"<td>{_context.Foods.FirstOrDefault(x => x.FoodsId == item.FoodItemId)!.Title}</td>\n");
                    stringBuilder.Append($"<td>{item.Qty}</td>\n");
                    stringBuilder.Append($"<td>$ {_context.Foods.FirstOrDefault(x => x.FoodsId == item.FoodItemId)!.Price}</td>\n");
                    stringBuilder.Append($"<td>$ {item.Qty * _context.Foods.FirstOrDefault(x => x.FoodsId == item.FoodItemId)!.Price}</td>\n");
                    stringBuilder.Append("</tr>\n");
                }
            }
            else
            {
                stringBuilder.Append("<tr>\n");
                stringBuilder.Append("<td></td>\n");
                stringBuilder.Append("<td></td>\n");
                stringBuilder.Append("<td></td>\n");
                stringBuilder.Append("<td></td>\n");
                stringBuilder.Append("<td></td>\n");
                stringBuilder.Append("</tr>\n");
            }

            stringBuilder.Append("</tbody>\n");
            stringBuilder.Append("</table>\n");
            stringBuilder.Append("<script src=\"https://cdn.jsdelivr.net/npm/@popperjs/core@2.11.8/dist/umd/popper.min.js\"\r\n        integrity=\"sha384-I7E8VVD/ismYTF4hNIPjVp/Zjvgyol6VFvRkX/vR+Vc4jQkC+hVqc2pM8ODewa9r\"\r\n        crossorigin=\"anonymous\"></script>");
            stringBuilder.Append("<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.min.js\"\r\n        integrity=\"sha384-fbbOQedDUMZZ5KreZpsbe1LCZPVmfTnH7ois6mU1QK+m14rQ1l2bGBq41eYeM/fS\"\r\n        crossorigin=\"anonymous\"></script>");
            stringBuilder.Append("</body>\n");
            stringBuilder.Append("</html>\n");

            // Convert StringBuilder to a string
            string htmlString = stringBuilder.ToString();

            var document = new Document();
            var outputStream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, outputStream);
            document.Open();

            using (var sr = new StringReader(htmlString))
            {
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
            }
            document.Close();

            // Get the PDF bytes
            byte[] pdfBytes = outputStream.ToArray();
            return pdfBytes;

        }
    }

    public interface IPDFservice
    {
        byte[] GenerateRecipte(ReceiptDto receipt);
    }
}
