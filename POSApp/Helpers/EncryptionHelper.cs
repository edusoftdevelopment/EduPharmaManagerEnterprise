using System;
using Microsoft.VisualBasic;

namespace POSApp.Services;

public class EncryptionHelper
{
    public static string eduEncrypt(string str2Encrypt, int EnCodedSize, bool Original)
    {
        int counter = 0;
        string tempStr = null;
        int TempSize = 0;
        const int Encryptwith = 3;
        if (Strings.Len(str2Encrypt) > EnCodedSize)
            str2Encrypt = str2Encrypt.Substring(0, EnCodedSize);
        tempStr = "";
        for (counter = 1; counter <= str2Encrypt.Length; counter++)
        {
            tempStr = tempStr + Strings.Chr(Strings.Asc(Strings.Mid(str2Encrypt, counter, 1)) ^ Encryptwith);
        }
        if (tempStr.Length < EnCodedSize)
            tempStr = tempStr + Strings.Chr(128);
        if (Original == false)
        {
            if (Strings.Len(tempStr) < EnCodedSize)
            {
                TempSize = tempStr.Length;
                for (counter = TempSize + 1; counter <= EnCodedSize; counter++)
                {
                    tempStr = tempStr + Strings.Chr(Convert.ToInt32((VBMath.Rnd() * 75)) + 40);
                }
            }
        }
        return tempStr;
    }
    public static string eduDecrypt(string str2Decrypt)
    {
        string functionReturnValue = null;
        int counter = 0;
        const int Decryptwith = 3;
        Strings.InStr(1, str2Decrypt, "''");
        string chars = Strings.Chr(128).ToString();
        if (Strings.InStr(1, str2Decrypt, Strings.Chr(128).ToString()) > 0)
            str2Decrypt = Strings.Left(str2Decrypt, Strings.InStr(1, str2Decrypt, Strings.Chr(128).ToString()) - 1);
        functionReturnValue = "";
        for (counter = 1; counter <= Strings.Len(str2Decrypt); counter++)
        {
            functionReturnValue = functionReturnValue + Strings.Chr(Strings.Asc(Strings.Mid(str2Decrypt, counter, 1)) ^ Decryptwith);
        }
        return functionReturnValue;
    }
}