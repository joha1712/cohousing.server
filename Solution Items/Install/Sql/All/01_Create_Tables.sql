CREATE TABLE IF NOT EXISTS address (
	id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
	streetName TEXT COLLATE "da-DK-x-icu" NOT NULL,
	streetNo VARCHAR(50) COLLATE "da-DK-x-icu" NOT NULL,
	floor VARCHAR(50) COLLATE "da-DK-x-icu" NOT NULL,
	side VARCHAR(50) COLLATE "da-DK-x-icu" NULL,
	callName TEXT COLLATE "da-DK-x-icu" NOT NULL
 );


CREATE TABLE IF NOT EXISTS person (
	id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
	firstName TEXT COLLATE "da-DK-x-icu" NOT NULL,
	lastName TEXT COLLATE "da-DK-x-icu" NOT NULL,
	callName TEXT COLLATE "da-DK-x-icu" NOT NULL,
	active BOOLEAN NOT NULL DEFAULT true,
	addressId INT REFERENCES address(id),
	attributes TEXT COLLATE "da-DK-x-icu" NULL
);

CREATE INDEX IF NOT EXISTS idx_person_addressId ON person(addressId);

CREATE TABLE IF NOT EXISTS commonMeal (
	id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
	date TIMESTAMP WITH TIME ZONE NOT NULL,
  note TEXT COLLATE "da-DK-x-icu" NULL,
	status TEXT COLLATE "da-DK-x-icu" NOT NULL DEFAULT 'OPEN'
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_commonMeal_data ON commonMeal(date);

CREATE TABLE IF NOT EXISTS commonMealRegistration (
	id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
	personId INT REFERENCES person(id) NOT NULL,
	commonMealId INT REFERENCES commonMeal(id) NOT NULL,
	attending BOOLEAN NOT NULL DEFAULT false,
	guests TEXT COLLATE "da-DK-x-icu" NULL,
	date TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
 );

CREATE TABLE IF NOT EXISTS commonMealChef (
	id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
	personId INT REFERENCES person(id) NULL,
	commonMealId INT REFERENCES commonMeal(id) NOT NULL,
	date TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS config (
	id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
	key varchar(100) NOT NULL,
	value TEXT COLLATE "da-DK-x-icu" NULL,
	description TEXT COLLATE "da-DK-x-icu" NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_config_key ON config(key);

CREATE TABLE IF NOT EXISTS commonMealExpense (
	id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
	personId INT REFERENCES person(id) NULL,
	commonMealId INT REFERENCES commonMeal(id) NOT NULL,
	amount REAL NOT NULL,
	date TIMESTAMP WITH TIME ZONE NOT NULL
);

CREATE TABLE IF NOT EXISTS community (
	id INT GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
	name TEXT COLLATE "da-DK-x-icu" NOT NULL,  
	schemaName TEXT COLLATE "da-DK-x-icu" NOT NULL
);

