CREATE PROCEDURE ClassRegistrationReport
AS
select * from Teacher
select * from Class
select * from ClassRegistration

select 'Class Name' = ClassName, 
	'Teacher Name' = TeacherName, 
	'Registrations' = count(Student_ID),
	'Number Paid' = sum(case when HasPaidFees = 1 then 1 else 0 end)
from Class
	inner join Teacher  on
		Class.Teacher_ID = Teacher.Teacher_ID
	left outer join ClassRegistration  on
		Class.Class_ID = ClassRegistration.Class_ID
group by ClassName, TeacherName
GO