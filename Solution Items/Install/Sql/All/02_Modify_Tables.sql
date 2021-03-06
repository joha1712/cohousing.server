-- noinspection SqlNoDataSourceInspectionForFile

-- noinspection SqlDialectInspectionForFile

ALTER TABLE commonMeal ADD COLUMN IF NOT EXISTS note TEXT COLLATE "da-DK-x-icu" NULL;
ALTER TABLE commonMeal ADD COLUMN IF NOT EXISTS status TEXT COLLATE "da-DK-x-icu" NOT NULL DEFAULT 'OPEN';
ALTER TABLE commonMealRegistration ADD COLUMN IF NOT EXISTS guests TEXT COLLATE "da-DK-x-icu" NULL;
ALTER TABLE commonMealRegistration ADD COLUMN IF NOT EXISTS date TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP;
ALTER TABLE commonMealRegistration ADD COLUMN IF NOT EXISTS takeaway BOOLEAN NOT NULL DEFAULT false;

ALTER TABLE person ADD COLUMN IF NOT EXISTS attributes TEXT COLLATE "da-DK-x-icu" NULL;

DROP TABLE IF EXISTS commonMealGuestRegistration;