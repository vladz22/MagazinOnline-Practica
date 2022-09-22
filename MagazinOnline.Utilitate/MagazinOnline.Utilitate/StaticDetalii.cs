using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazinOnline.Utilitate
{
    public static class StaticDetalii
    {   //roles
        public const string Rol_User_Indiv = "Client Individual";
        public const string Rol_User_Comp = "Client Companie";
        public const string Rol_Admin = "Admin";
        public const string Rol_Angajat = "Angajat";
        //sesiunea
        public const string ssShoppingCart = "ShoppingCartSession";
        //Status comanda
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCanceled = "Canceled";
        public const string StatusRefunded = "Refunded";
        //Status Payment
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";


        public static double PretInFunctieDeCantitate(double cantitate, double pret, double pret5)
        {
            if(cantitate<5)
            {
                return pret;
            }
            else 
                {
                    return pret5;
                }    
        }
        public static string ConvertToRawHtml(string source)//aproape acelasi lucru ca @Html.Raw()
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

    }
}
