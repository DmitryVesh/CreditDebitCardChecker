using System;
using System.IO;

namespace CreditCardCheck
{
    class Program
    {
        static string filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../")) + @"IIN.csv";
        private static void DetailCard(string cardNum) //outputing known information about the card
        {
            Console.WriteLine("\nAttempting to find details... Please wait...");
            string cardNum6LengthStr = "", cardNum8LengthStr = ""; // The starting lengths only include the first 6 and 8 characters of a card
            for (int count = 0; count < 9; count++)
            {
                if (count < 6)
                {
                    cardNum6LengthStr += cardNum[count];
                    cardNum8LengthStr += cardNum[count];
                }
                else { cardNum8LengthStr += cardNum[count]; }
            }
            int cardNum6LengthInt = Convert.ToInt32(cardNum6LengthStr);
            int cardNum8LengthInt = Convert.ToInt32(cardNum8LengthStr);


            string foundDetails = "";
            bool detailsPresent = false;
            StreamReader reader = new StreamReader(File.OpenRead(filePath));
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var values = line.Split(',');

                try
                {
                    for (int count = 0; count <= (Convert.ToInt32(values[1]) - Convert.ToInt32(values[0])); count++)
                    {
                        if (Convert.ToInt32(values[0]) + count == cardNum6LengthInt || Convert.ToInt32(values[0]) + count == cardNum8LengthInt)
                        { detailsPresent = true; foundDetails = line; }
                    }
                }
                catch (FormatException) { if (values[0] == cardNum6LengthStr || values[0] == cardNum8LengthStr) { detailsPresent = true; foundDetails = line; } }
                if (detailsPresent == true) { break; }

            }


            if (detailsPresent == false) { Console.WriteLine("\nDetails of the card have not been found."); }
            else
            {

                var values = foundDetails.Split(',');
                string[] details = { "scheme", "brand", "type", "prepaid", "country", "bank name", "bank logo", "URL of the bank", "phone number of the bank", "city of the bank" };

                Console.WriteLine("\n----------------------------------------------------------------------------");
                Console.WriteLine("\nDetails about card:\n");
                for (int count = 4; count < 14; count++)
                {
                    if (values[count] != "") { Console.WriteLine("The {0} is : {1}", details[count - 4], values[count]); }
                }
                Console.WriteLine("\n----------------------------------------------------------------------------");
            }
        }
        private static void CheckCard(string cardNum)
        { //checking validity of the card

            int charValue, sumOfCard = 0, loopCount = 0;

            for (int forLoop = 15; forLoop >= 0; forLoop--)
            {
                loopCount += 1;
                charValue = int.Parse(cardNum[forLoop].ToString());

                if (loopCount % 2 == 0)
                {
                    charValue = charValue * 2;
                    if (charValue > 9) { charValue -= 9; }
                    sumOfCard += charValue;
                }

                else { sumOfCard += charValue; }
            }

            if (sumOfCard % 10 == 0)
            {
                Console.WriteLine("\nThe card number entered is valid.");
                DetailCard(cardNum);
            }
            else { Console.WriteLine("\nThe card number entered is invalid."); }
        }


        public static void Main() //main
        {
            // xxxx,xxxx,xxxx,xxxx
            string cardNum;
            while (true)
            {
                Console.Write("\nEnter a card number: ");
                cardNum = Console.ReadLine();
                if (cardNum.Length != 16) { Console.WriteLine("\nError, card number must be 16 characters long. Try again."); }
                else if (!double.TryParse(cardNum, out _)) { Console.WriteLine("\nError, card number contains other characters apart from numbers. Try again."); }
                else { CheckCard(cardNum); }
            }
        }
    }
}
