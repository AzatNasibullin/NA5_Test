using EFSeminar.Models;
using Microsoft.EntityFrameworkCore;
using NA5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NA5.Services
{
    internal class MessageServices
    {
        public List<Message> GetUnsentMessages()
        {
            using (var db = new ChatContext())
            {
                var unreadMessages = db.Messages.Where(m => !m.IsSent).ToList();

                return unreadMessages;
            }
        }

        public void PrintUnsentMessages()
        {
            List<Message> unsentMessages = GetUnsentMessages();

            foreach (Message message in unsentMessages)
            {
                Console.WriteLine(message.ToString());
            }
        }

        public List<Message> GetUnreadMessages()
        {
            using (var db = new ChatContext())
            {
                var unreadMessages = db.Messages.Where(m => !m.IsRead).ToList();

                return unreadMessages;
            }
        }

        public void PrintUnreadMessages()
        {
            List<Message> unreadMessages = GetUnreadMessages();

            foreach (Message message in unreadMessages)
            {
                Console.WriteLine(message.ToString());
            }
        }

        public void PullMessages()
        {
            using (var ChatCtx = new ChatContext())
            {
                User Ivan = new User { FullName = "Ivan" };
                User Igor = new User { FullName = "Igor" };
                var messages = new List<Message>
                {
                    new Message { IsRead = false,IsSent = true, Text = "Привет!", UserFrom = Ivan, UserTo = Igor },
                    new Message { IsRead = false,IsSent = true, Text = "И тебе привет!", UserFrom = Igor, UserTo = Ivan },
                    // Добавьте другие сообщения здесь
                };

                ChatCtx.Messages.AddRange(messages);
                try
                {
                    ChatCtx.SaveChanges();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("Ошибка соединения с бд: " + e.Message);
                    throw;
                }
                catch (DbUpdateException e)
                {
                    Console.WriteLine("Ошибка сохранения изменений в бд: " + e.Message);
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Неизвестная ошибка: " + e.Message);
                    throw;
                }
            }
        }
    }
}
