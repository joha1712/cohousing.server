CREATE OR REPLACE FUNCTION commonmeal_statistics_overview(fromDate timestamp with time zone, toDate timestamp with time zone)
  RETURNS TABLE (personId INTEGER, chefCount INTEGER, mealCount INTEGER, adultGuestsCount INTEGER, childGuestsCount INTEGER, expensesSum REAL) AS
$func$
BEGIN
   RETURN QUERY
   WITH cte AS (
		SELECT 
			cmr.personId, 
			(CAST (SUBSTRING(guests FROM 'ADULTS,CONVENTIONAL\=(.*?);') AS INTEGER) + CAST (SUBSTRING(guests FROM 'ADULTS,VEGETARIAN\=(.*?);') AS INTEGER)) AS adult_guests,
			(CAST (SUBSTRING(guests FROM 'CHILDREN,CONVENTIONAL\=(.*?);') AS INTEGER) + CAST (SUBSTRING(guests FROM 'CHILDREN,VEGETARIAN\=(.*?);') AS INTEGER)) AS child_guests,
			CAST(attending AS INTEGER) AS attending,
			commonmealId
		FROM commonmealRegistration cmr
	   	INNER JOIN commonMeal on commonMeal.id = cmr.commonMealId
	   	WHERE commonMeal.date >= fromDate AND commonMeal.date <= toDate
   	),
	cte2 AS (
		SELECT *,
		(
			SELECT
			COUNT(*)
				FROM
					commonmealchef cmc
				WHERE
					cmc.commonmealId=cte.commonmealId AND cmc.personId = cte.personId
			) as chefs
		FROM cte
	),
	cte3 AS (
		SELECT *,
		(
			SELECT
			SUM(cme.amount)
				FROM
					commonMealExpense cme
				WHERE
					cme.commonmealId=cte2.commonmealId AND cme.personId = cte2.personId
			) as expenses
		FROM cte2
	) 
	SELECT cte3.personId, 
		CAST(SUM(coalesce(chefs,0)) AS INTEGER) AS chefCount, 
		CAST(SUM(coalesce(attending,0)) AS INTEGER) AS mealCount, 
		CAST(SUM(coalesce(adult_guests,0)) AS INTEGER) AS adultGuestsCount, 
		CAST(SUM(coalesce(child_guests,0)) AS INTEGER) AS childGuestsCount,
		CAST(SUM(coalesce(expenses,0.0)) AS REAL) AS expensesSum
	FROM cte3
	GROUP by cte3.personid
	ORDER BY cte3.personId;
END
$func$  LANGUAGE plpgsql;