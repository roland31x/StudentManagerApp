﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;

namespace StudentManagerApp.PersonClasses
{
    public interface IPerson
    {
        public void Validate();
        public void MinimalListThis(StackPanel list);

    }
    abstract public class Person : IPerson, INotifyPropertyChanged
    {
        public static Dictionary<int, Person> FullPersonList = new Dictionary<int, Person>();
        public string[] FullName { get; protected set; }
        public int Id { get; protected set; }
        public string TrueID { get { return Id.ToString("D7"); } }
        public string? Email { get; protected set; }
        public string FirstName { get { return FullName.First(); } set { FullName[0] = value; Name = string.Empty; } }
        public DateTime? DateOfBrith { get; protected set; }
        public string LastName { get { return FullName.Last(); } set { FullName[1] = value; Name = string.Empty; } }
        public string Name { get { return FirstName + " " + LastName; } set { _ = value; OnPropertyChanged(); } }
        public string? PersonalEmail { get; protected set; }
        public string? Phone { get; protected set; }
        public abstract string Function { get; protected set; }
        public string WasFullyValidated { get; protected set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public static void MinimalListItems<IPerson>(StackPanel panel)
        {
            foreach (Person type in FullPersonList.Values.Where(x => x is IPerson))
            {
                type.MinimalListThis(panel);
            }
        }
        protected Person(int ID, string name)
        {
            FullPersonList.Add(ID, this);
            string[] temp = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            FullName = new string[] { name.Replace(temp.Last(), "").TrimEnd().TrimStart(), temp.Last() };
            Id = ID;
            WasFullyValidated = "!";
        }
        public void SetPersonalEmail(string email)
        {
            PersonalEmail = email;
        }
        public void SetPhone(string phone)
        {
            Phone = phone;
        }
        public void SetDoB(DateTime date)
        {
            DateOfBrith = date;
        }
        public static Person FindPersonAfterID(int ID)
        {
            return FullPersonList[ID];
        }
        public static void RemovePerson(Person p)
        {
            FullPersonList.Remove(p.Id);
        }
        public static void RemovePersonAfterID(int ID)
        {
            FullPersonList.Remove(ID);
        }
        protected virtual bool TryValidate()
        {
            bool Valid = true;
            string[] PersonName = FullName;
            if (PersonName.Length == 1)
            {
                throw new ArgumentException("Invalid Person name!!!");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(PersonName.First()).Append('-').Append(PersonName.Last()).Append('@').Append(Function.ToLower()).Append(".MyService.com");
                Email = sb.ToString();
            }
            if (PersonalEmail == null || !PersonalEmail.Contains('@'))
            {
                Valid = false;
            }
            if (Phone == null)
            {
                Valid = false;
            }
            if (DateOfBrith == null)
            {
                Valid = false;
            }

            return Valid;
        }
        public abstract void MinimalListThis(StackPanel list);
        public abstract void Validate();
        public static void BindToControlElement(Control control, Person source, string PropertyName)
        {
            Binding myBind = new Binding(PropertyName);
            myBind.Source = source;
            BindingOperations.SetBinding(control, ContentControl.ContentProperty, myBind);
        }
    }
   
}