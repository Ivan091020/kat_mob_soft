-- Проверка наличия столбцов email_confirmed и email_confirmation_token
SELECT 
    column_name, 
    data_type, 
    is_nullable, 
    column_default
FROM information_schema.columns
WHERE table_schema = 'public' 
  AND table_name = 'users'
  AND column_name IN ('email_confirmed', 'email_confirmation_token')
ORDER BY column_name;

