using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BillingSystem.Contracts
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T data { get; set; }
        public HttpStatusCode StatusCode { get; set; } 
        public string Message { get; set; } 
        public List<string> ErrorList { get; set; }= new List<string>();


        public ApiResponse()
        {
            IsSuccess = true;
            StatusCode= HttpStatusCode.OK;
        }
        public ApiResponse(T data,string message="",HttpStatusCode statusCode = HttpStatusCode.OK) 
        {
            IsSuccess = true;
            this.data = data;
            this.Message = message;
            StatusCode = statusCode;
            ErrorList = null;
        }
        //For single error
        public ApiResponse(string errorMsg,HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            ErrorList.Add(errorMsg);
            Message = null;
        }
        // For multiple errors (List)
        public ApiResponse(List<string> errorList, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            ErrorList = errorList;
            Message=null;
        }


    }



}
