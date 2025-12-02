-- Миграция для добавления полей подтверждения email
-- Выполните этот SQL скрипт в вашей базе данных PostgreSQL

-- Добавляем столбец email_confirmed
ALTER TABLE public.users 
ADD COLUMN IF NOT EXISTS email_confirmed BOOLEAN NOT NULL DEFAULT false;

-- Добавляем столбец email_confirmation_token
ALTER TABLE public.users 
ADD COLUMN IF NOT EXISTS email_confirmation_token VARCHAR(500);

-- Проверяем результат
SELECT column_name, data_type, is_nullable, column_default
FROM information_schema.columns
WHERE table_schema = 'public' 
  AND table_name = 'users'
  AND column_name IN ('email_confirmed', 'email_confirmation_token');

