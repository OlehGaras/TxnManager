# TxnManager
> Web application for transactions import and management

## How to run?
- Open TxnManager.sln file in visual studio;
- Run "update-database" command in power shell window => db should be created;
- Run F5;
- Open browser in localhost:5000;

## Upload transactions file
Supported file extensions: 
- XML
```sh
<?xml version="1.0" encoding="UTF-8"?>
<Transactions>
   <Transaction id="Inv00001">
      <TransactionDate>2019-01-24T16:09:15</TransactionDate>
      <PaymentDetails>
         <Amount>100600.00</Amount>
         <CurrencyCode>EUR</CurrencyCode>
      </PaymentDetails>
      <Status>Done</Status>
   </Transaction>
   <Transaction id="Inv00002">
      <TransactionDate>2019-01-26T16:09:15</TransactionDate>
      <PaymentDetails>
         <Amount>200400.00</Amount>
         <CurrencyCode>USD</CurrencyCode>
      </PaymentDetails>
      <Status>Rejected</Status>
   </Transaction>
</Transactions>
```
- CSV
```sh
"Invoice0000001","123,500.00","USD","21/02/2019 02:04:59","Approved"
"Invoice0000002","1,300.00","EUR","18/03/2019 02:04:59","Approved"
```

## Get transactions using API
Web api implemented for getting transactions by filters:
- All (without specifying any filters);
```sh
http://localhost:5000/api/transactions
```
- Currency (iso4217 standard: USD, EUR);
```sh
http://localhost:5000/api/transactions?currency=eur
```
- Status (A,R,D);
```sh
http://localhost:5000/api/transactions?status=a
```
- Date range;
```sh
http://localhost:5000/api/transactions?range.from=2019-01-29&range.to=2019-02-28
```
- Combination of filters:
```sh
http://localhost:5000/api/transactions?currency=eur&status=a
http://localhost:5000/api/transactions?currency=eur&range.from=2019-01-29&range.to=2019-02-28
http://localhost:5000/api/transactions?currency=eur&status=r&range.from=2019-01-29&range.to=2019-02-28
```
