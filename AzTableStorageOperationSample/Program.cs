// This is a simple sample program that uses Microsoft.Azure.Cosmos.Table to work with Azure Storage Table.
// This console application how to use is the following:
// 
// [Preparation before build]
// You should write connection string of your any Azure storage account to the  Setting.json in this project.
//
// [How to use]
// This sample program does support feature is create and delete table, add and remove an entity and enumerates entities.
//
// Feature and command line arguments:
// ・Create table : create <table name>
// ・Delete table : delete <table name>
// ・Add entity : add <firstname>,<lastname>,<email address>,<phone number>
// ・Delete entity : remove <firstname>,<lastname>
// ・Display entity : show <firstname>,<lastname>
// ・Enumerates entity : enum <table name> or enum <table name> <number of takes>
//
// You able to set these command line(Application) arguments using [Debug] tab in the project property view.
//
// Written by Osamu Monoe.

using System;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;

namespace AzTableStorageOperationSample
{
    
    class Program
    {
       static CloudStorageAccount storageAccount;
       static CloudTableClient tableClient;
        static void Main(string[] args)
        {
            // Get connection string for connect to the Azure Storage Account.
            string storageConnectionString = AppSettings.LoadAppSettings().StorageConnectionString;

            storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create the tableClient object.
            tableClient = storageAccount.CreateCloudTableClient();

            //Routing to each feature
            switch (args[0])
            {
                case "create":
                    Console.WriteLine(createTable(args[1]));
                    break;
                case "delete":
                    Console.WriteLine(deleteTable(args[1]));
                    break;
                case "add":
                    Console.WriteLine(addEntity(args[1], args[2]));
                    break;
                case "remove":
                    Console.WriteLine(deleteEntry(args[1], args[2]));
                    break;
                case "show":
                    string[] entityPropArray = showEntity(args[1], args[2]);
                    Console.WriteLine("FirstName : {0}", entityPropArray[0]);
                    Console.WriteLine("LastName : {0}", entityPropArray[1]);
                    Console.WriteLine("EMail Address : {0}", entityPropArray[2]);
                    Console.WriteLine("PhoneNumber : {0}", entityPropArray[3]);
                    break;
                case "enum":
                    Console.WriteLine(enumEntity(args[1], int.Parse(args[2])));
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }

        //Create a table.
        static string createTable(string tableName)
        {
            string returnString = "[done] Created table.";
            try
            {
                // Retrieve a reference to the table.
                CloudTable table = tableClient.GetTableReference(tableName);
                // Create the table if it doesn't exist.
                table.CreateIfNotExists();
            }
            catch (Exception err)
            {
                returnString = err.Message;
            }
            return returnString;
        }

        //Delete a table.
        static string deleteTable(string tableName)
        {
            string returnString = "[done] Deleted table.";
            try
            {
                // Create the CloudTable object that represents the table name in tableName variable.
                CloudTable table = tableClient.GetTableReference(tableName);

                // Delete the table it if exists.
                table.DeleteIfExists();
            }
            catch (Exception err)
            {
                returnString = err.Message;
            }
            return returnString;
        }

        //Add an entity.
        static string addEntity(string tableName, string csvArg)
        {
            string returnString = "[done] Added entity. ";
            try
            {
                string[] argArray = csvArg.Split(',');
                string firstName = argArray[0];
                string lastName = argArray[1];
                string mailAddress = argArray[2];
                string phoneNumber = argArray[3];

                // Create the CloudTable object that represents the table name in tableName variable.
                CloudTable table = tableClient.GetTableReference(tableName);

                // Create a new customer entity.
                CustomerEntity customer1 = new CustomerEntity(firstName, lastName);
                customer1.Email = mailAddress;
                customer1.PhoneNumber = phoneNumber;

                // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(customer1);

                // Execute the insert operation.
                table.Execute(insertOperation);
            }
            catch (Exception err)
            {
                returnString = "[fail] " + err.Message;
            }
            return returnString;
        }

        //Delete an entity.
        static string deleteEntry(string tableName, string csvArg)
        {
            string returnString = "[done] Added entity. ";
            try
            {
                string[] argArray = csvArg.Split(',');
                string firstName = argArray[0];
                string lastName = argArray[1];

                // Create the CloudTable object that represents the table name in tableName variable.
                CloudTable table = tableClient.GetTableReference(tableName);

                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(firstName, lastName);

                // Execute the retrieve operation.
                TableResult retrievedResult = table.Execute(retrieveOperation);
                // Assign the result to a CustomerEntity.
                CustomerEntity deleteEntity = (CustomerEntity)retrievedResult.Result;

                // Create the Delete TableOperation.
                if (deleteEntity != null)
                {
                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                    // Execute the operation.
                    table.Execute(deleteOperation);
                }
                else
                {
                    returnString = "Could not retrieve the entity.";
                }
            }
            catch (Exception Err)
            {
                returnString = "[fail] " + Err.Message;
            }
            return returnString;
        }

        // Show an entity information.
        static string[] showEntity(string tableName, string csvArg)
        {

            string[] argArray = csvArg.Split(',');
            string firstName = argArray[0];
            string lastName = argArray[1];

            // Create the CloudTable object that represents the table name in tableName variable.
            CloudTable table = tableClient.GetTableReference(tableName);

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(firstName, lastName);

            // Execute the retrieve operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Print the phone number of the result.
            if (retrievedResult.Result != null)
            {
                CustomerEntity entity = (CustomerEntity)retrievedResult.Result;
                return new string[] { firstName, lastName, entity.Email, entity.PhoneNumber };
            }
            else
            {
                return new string[] { "The phone number could not be retrieved." };
            }
        }

        //Enumerates a specified number of entities.
        static string enumEntity(string tableName, int numberOfTakes=-1)
        {
            string returnString = "[done] Enumlated entities.";
            try {
                // Create the CloudTable object that represents the table name in tableName variable.
                CloudTable table = tableClient.GetTableReference(tableName);

                TableQuery<CustomerEntity> query = (numberOfTakes == 1) ?
                    new TableQuery<CustomerEntity>() : new TableQuery<CustomerEntity>().Take(numberOfTakes);

                foreach (CustomerEntity entity in table.ExecuteQuery(query))
                {
                    Console.WriteLine("FirstName : {0}", entity.PartitionKey);
                    Console.WriteLine("LastName : {0}", entity.RowKey);
                    Console.WriteLine("EMail Address : {0}", entity.Email);
                    Console.WriteLine("PhoneNumber : {0}", entity.PhoneNumber);
                    Console.WriteLine("---------------");
                }
            }
            catch (Exception Err)
            {
                returnString = "[fail] " + Err.Message;
            }
            return returnString;
        }

    }


    
    //Define a entity.
    public class CustomerEntity : TableEntity
    {
        public CustomerEntity(string lastName, string firstName)
        {
            this.PartitionKey = lastName;
            this.RowKey = firstName;
        }

        public CustomerEntity() { }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
