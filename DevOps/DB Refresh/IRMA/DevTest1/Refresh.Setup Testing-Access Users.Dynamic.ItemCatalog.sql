use ItemCatalog;

print 'Update IDF AllowChangeOwnTitle=1'
update instancedataflags set flagvalue = 1 where flagkey = 'AllowChangeOwnTitle'

set nocount on
declare @useradd table (username varchar(128) not null, fullname varchar(128) not null)
insert into @useradd values
('Benjamin.Sims', 'Benjamin Sims'),
('SteffeB', 'Bobbie Steffe'),
('MillerDan', 'Dan Miller'),
('Min.Zhao', 'Min Zhao'),
('Peggy.Brauer', 'Peggy Brauer'),
('Tom.Lux', 'Tom Lux'),
('Odunayo.Olanrewaju', 'Odunayo Olanrewaju'),
('Kevin.Williams1', 'Kevin Williams'),
('aldrin.bendo', 'Aldrin Bendo'),
('alpesh.patel', 'Alpesh Patel'),
('arturo.rivas', 'Arturo Rivas'),
('2120808', 'Ahmad Abouhosah'),
('2096654', 'Amit Uppal'),
('EudyR', 'Robin Eudy'),
('Chloe.Gadok', 'Chloe Gadok'),
('2195849', 'Stephanie Villarreal'),
('2202835', 'Maggie Mitchell'),
('2202809', 'Travis Taylor'),
('2202808', 'Shreedam Parikh'),
('AhmedFa', 'Faisal Ahmed'),
('Denis.Ng', 'Denis Ng'),
('LiiasE', 'Egan Liias'),
('anitha.rajendran', 'Anitha Rajendran'),
('2145269', 'Cary Conklin'),
('ChapmanRu', 'Ruth Chapman'),
('HarpS', 'Shon Harp'),
('MitchellCa', 'Caroline Mitchell'),
('Habib.Azzam', 'Habib Azzam'),
('Kenneth.Blankertz', 'Kenneth Blankertz'),
('GustafsK', 'Kristin Gustafson'),
('SistarP', 'Paul Sistare'),
('SantosE', 'Erick Dos Santos'),
('Erik.Vandivier', 'Erik Vandivier'),
('Anthony.Marsella', 'Anthony Marsella'),
('Ashley.Bushi', 'Ashley Bushi'),
('Charles.Shaw', 'Charles Shaw'),
('lowed', 'David Lowe'),
('santose', 'Erick Dos Santose'),
('Gerrit.VanVoorst', 'Gerrit VanVoorst'),
('Karin.Edwards', 'Karin Edwards'),
('Theresa.Valadez', 'Theresa Valadez'),
('Yegor.Filonov', 'Yegor Filonov'),
('Marshall.Chappell', 'Marshall Chappell'),
('melissa.minor', 'Melissa Minor'),
('2093892', 'Meenakshi Vishwanath'),
('Bailey.Ertel', 'Bailey Ertel'),
('2114273', 'Hailey Pate'),
('falesa', 'Alice Fales'),
('charles.shaw', 'Charlie Shaw'),
('2141195', 'Sujoy Ganguly'),
('2141197', 'Dinesh Seralathan'),
('2141196', 'Venkata Elchuri'),
('Karen.Castillo', 'Karen Castillo'),
('2073615', 'Ram Karamchandani'),
('2091952', 'Sally Merritt'),
('2091951', 'Brenton Reittinger'),
('2072153', 'Swapna Panguluri'),
('2078243', 'Nikhil Thirunagori'),
('2079681', 'Valli Ashok'),
('2086768', 'Tong Suasin'),
('dot', 'Tom Do'),
('wardch', 'Chris Ward'),
('billy.blackerby', 'Billy Blackerby'),
('joseph.witherington', 'Joseph Witherington'),
('2091948', 'Willie Camayang'),
('2116648', 'Rajkumar Wilson'),
('2116650', 'Jitin Varghese'),
('2101369', 'Piyush Samal'),
('2111973', 'Dustin Oatley'),
('2107026', 'Smita Singh'),
('Trey.Damico', 'Trey DAmico'),
('2171282', 'Obaid Safdar'),
('2186912', 'Harikishore Bachina'),
('2165775', 'Orosz Rachel'),
('2179239', 'Ryan Tansey'),
('2179257', 'Nathan Steele'),
('2116649', 'Preeti Banerjee'),
('2109365', 'Sakthi Shanmugavel'),
('2080986', 'Basanth Sampangi'),
('2143118', 'Himanshu Kumar'),
('2189246', 'Jyoti Gaba'),
('Mikaela.Hensen', 'Mikaela Hensen'),
('2159699', 'Leo Gordillo'),
('2192362', 'VAna Abbott'),
('2192361', 'Romel Benavides'),
('2203466', 'Suruchi Sharma')


while exists (select top 1 * from @useradd)
begin
	declare @fullname varchar(128), @username varchar(128)
	select top 1 @username = username, @fullname = fullname from @useradd
	if not exists (select username from users where username like @username) 
	begin
		print N'adding user: ' + @fullname + '(' + @username + ')'
		insert into users (username, fullname, accountenabled, securityadministrator) values (@username, @fullname, 1, 1);
	end
	else
	begin
		if exists (select username from users where username like @username and (accountenabled = 0 or securityadministrator = 0))
		begin
			print N'updating user: ' + @fullname + '(' + @username + ')'
			update users set accountenabled = 1, securityadministrator = 1 where username like @username and (accountenabled = 0 or securityadministrator = 0);
		end
	end

	--print N'delete working-list entry'
	delete from @useradd where username = @username
end
