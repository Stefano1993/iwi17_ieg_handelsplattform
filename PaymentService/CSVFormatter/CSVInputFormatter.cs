using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using PaymentService.Controllers.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.CSVFormatter
{
    public class CsvInputFormatter : TextInputFormatter
    {

        public CsvInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));

        }
        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)

        {
            var type = context.ModelType;
            var request = context.HttpContext.Request;
            MediaTypeHeaderValue requestContentType = null;
            MediaTypeHeaderValue.TryParse(request.ContentType, out requestContentType);

            var result = ReadStream(type, request.Body);
            return InputFormatterResult.SuccessAsync(result);
        }

        public override bool CanRead(InputFormatterContext context)
        {
            var type = context.ModelType;
            if (typeof(Payment).IsAssignableFrom(type)
                || typeof(IEnumerable<Payment>).IsAssignableFrom(type))
            {
                return true;
            }
            return false; ;
        }


        private object ReadStream(Type type, Stream stream)
        {
            Type itemType;
            var typeIsArray = false;
            IList list;
            if (type.GetGenericArguments().Length > 0)
            {
                itemType = type.GetGenericArguments()[0];
                list = (IList)Activator.CreateInstance(type);
            }
            else
            {
                typeIsArray = true;
                itemType = type.GetElementType();

                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(itemType);

                list = (IList)Activator.CreateInstance(constructedListType);
            }


            var reader = new StreamReader(stream);

            bool skipFirstLine = true;
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(";".ToCharArray());
                if (skipFirstLine)
                {
                    skipFirstLine = false;
                }
                else
                {
                    var itemTypeInGeneric = list.GetType().GetTypeInfo().GenericTypeArguments[0];
                    var item = Activator.CreateInstance(itemTypeInGeneric);
                    var properties = item.GetType().GetProperties();
                    for (int i = 0; i < values.Length; i++)
                    {
                        properties[i].SetValue(item, Convert.ChangeType(values[i], properties[i].PropertyType), null);
                    }

                    list.Add(item);
                }

            }

            if (typeIsArray)
            {
                Array array = Array.CreateInstance(itemType, list.Count);

                for (int t = 0; t < list.Count; t++)
                {
                    array.SetValue(list[t], t);
                }
                return array;
            }

            return list;
        }

    }
}


