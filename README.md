## ReceiptManager

CRUD API application for managing receipts.
- This project uses SqlServer local database
- Unit tested via XUnit

---

### Description:

Functions:
- Create a new receipt
- Delete a receipt by id

- Get all receipts
- Get a receipt by id
- Get the list of receipts by contents or creation period

 An IDE software with C#/.NET support is necessary to run this project. 

---
How to run in Visual Studio:

1. Open the .sln file:
```
ReceiptManager.sln, here: ReceiptManager/ReceiptManager.sln
```
2. Once in Visual Studio, run the ReceiptManager project

```
You should see a Swagger page open with all of the endpoints 
note: keep the project running to send requests
```
3. Toggle any endpoint you wish

```
You should receive correct responses for your requests
```
4. Run tests in Visual Studio:

```
Test/Run All Tests
```

---


### Brief:

```
Description

The objective is to build a simple CRUD API to process Receipt (payment) resources. 
You can use either the REST API or any RPC (gRPC and so on) model. How the data is stored is up to you.

Please create the following API endpoints:

    - Create a new Receipt
    - Delete the Receipt by its id
    - Get the Receipt by its id
    - Get the list of Receipts

    - Get all Receipts
    - Get filtered Receipts by creation date range
    - Get filtered Receipts by product name - find all Receipts containing an item with the name containing text

“Receipt” Fields

    - Id - a unique identifier
    - CreatedOn - the date when the given Receipt was created on
    - Items - list of Receipt Items. Each item contains the following fields:

    - ProductName

```
