-- Fetch  product
SELECT p.ProductName, p.UnitPrice, c.CategoryName
FROM Products AS p
JOIN Categories AS c ON p.CategoryID = c.CategoryID
ORDER BY c.CategoryName, p.ProductName;

-- Fetch customers
SELECT c.CustomerID, c.CompanyName, COUNT(o.OrderID) AS NumberOfOrders
FROM Customers AS c
LEFT JOIN Orders AS o ON c.CustomerID = o.CustomerID
GROUP BY c.CustomerID, c.CompanyName
ORDER BY NumberOfOrders DESC;

-- Fetch employees
SELECT e.EmployeeID, e.FirstName, e.LastName, t.TerritoryDescription
FROM Employees AS e
JOIN EmployeeTerritories AS et ON e.EmployeeID = et.EmployeeID
JOIN Territories AS t ON et.TerritoryID = t.TerritoryID;
