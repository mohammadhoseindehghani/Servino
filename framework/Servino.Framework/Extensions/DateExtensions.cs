using System.Globalization;

namespace Servino.Framework.Extensions;

public static class DateExtensions
{
    /// <summary>
    /// Converts a Shamsi (Persian) date string to a Gregorian DateTime object.
    /// </summary>
    /// <param name="shamsiDateString">The Shamsi date string in "yyyy/MM/dd" or "yyyy-MM-dd" format.</param>
    /// <returns>A DateTime object representing the Gregorian date, or null if conversion fails.</returns>
    public static DateTime? ToGregorianDateTime(this string shamsiDateString)
    {
        if (string.IsNullOrWhiteSpace(shamsiDateString))
        {
            return null;
        }

        try
        {
            // Normalize the date string (replace '-' with '/')
            shamsiDateString = shamsiDateString.Replace('-', '/');

            PersianCalendar pc = new PersianCalendar();
            string[] dateParts = shamsiDateString.Split('/');

            if (dateParts.Length == 3)
            {
                int year = int.Parse(dateParts[0]);
                int month = int.Parse(dateParts[1]);
                int day = int.Parse(dateParts[2]);

                return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
        }
        catch (FormatException)
        {
            // Log or handle the format exception
            Console.WriteLine($"Invalid Shamsi date format: {shamsiDateString}");
        }
        catch (ArgumentOutOfRangeException)
        {
            // Log or handle out of range exception (e.g., invalid month/day)
            Console.WriteLine($"Shamsi date out of range: {shamsiDateString}");
        }
        catch (Exception ex)
        {
            // Catch any other unexpected exceptions
            Console.WriteLine($"An error occurred during date conversion: {ex.Message}");
        }

        return null; // Return null for any conversion failure
    }
}