using Microsoft.AspNetCore.Http;
using System;
public class CommonVariable
{
    private static IHttpContextAccessor _HttpContextAccessor;

    static CommonVariable()
    {
        _HttpContextAccessor = new HttpContextAccessor();
    }

    public static string JWTToken()
    {
        return _HttpContextAccessor.HttpContext.Session.GetString("JWTToken");
    }
    public static int? UserID()
    {

        if (_HttpContextAccessor.HttpContext.Session.GetString("UserID") == null)
        {
            return null;
        }

        return Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("UserID"));
    }

    public static string UserName()
    {
        if (_HttpContextAccessor.HttpContext.Session.GetString("Name") == null)
        {
            return null;
        }

        return _HttpContextAccessor.HttpContext.Session.GetString("Name");
    }

    public static string Email()
    {
        if (_HttpContextAccessor.HttpContext.Session.GetString("Email") == null)
        {
            return null;
        }
        return _HttpContextAccessor.HttpContext.Session.GetString("Email");
    }
    public static string Password()
    {
        if (_HttpContextAccessor.HttpContext.Session.GetString("Password") == null)
        {
            return null;
        }
        return _HttpContextAccessor.HttpContext.Session.GetString("Password");
    }
    public static int? FamilyGroupID()
    {
        if (_HttpContextAccessor.HttpContext.Session.GetString("FamilyGroupID") == null)
        {
            return null;
        }
        return Convert.ToInt32(_HttpContextAccessor.HttpContext.Session.GetString("FamilyGroupID"));
    }
    public static string GroupName()
    {
        if (_HttpContextAccessor.HttpContext.Session.GetString("GroupName") == null)
        {
            return null;
        }
        return _HttpContextAccessor.HttpContext.Session.GetString("GroupName");
    }
    public static string Role()
    {
        if (_HttpContextAccessor.HttpContext.Session.GetString("Role") == null)
        {
            return null;
        }
        return _HttpContextAccessor.HttpContext.Session.GetString("Role");
    }
}