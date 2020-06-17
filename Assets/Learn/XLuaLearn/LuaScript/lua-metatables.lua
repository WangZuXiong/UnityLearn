--Lua元表


myTable = {[1] = "11",[2] = "22"} --普通表
mymetatable = {[3] = "33",[4] = "44"} --元表
setmetatable(myTable,mymetatable) --把mymetatable设置为mytable的元素

print("myTable 元素1对应的value::"..myTable[1])--11
print("myTable 元素2对应的value::"..myTable[2])--22
--print("myTable 元素3对应的value::"..myTable[3])--attempt to concatenate a nil value
--print("myTable 元素4对应的value::"..myTable[4])--attempt to concatenate a nil value


mymetatable.__index = mymetatable
print("myTable 元素1对应的value::"..myTable[1])--11
print("myTable 元素2对应的value::"..myTable[2])--22
print("myTable 元素3对应的value::"..myTable[3])--33
print("myTable 元素4对应的value::"..myTable[4])--44

--以上代码也可以直接写成一行：

myTable = setmetatable({},{})

--以下为返回对象元表：
getmetatable(myTable) --这回返回mymetatable


--[[
local a = {}
local b = setmetatable({},{__index = a})
a.i = 10
print(b.i)
b.i = 20
print(a.i)
]]


-- __index 元方法
--[[
Lua 查找一个表元素时的规则，其实就是如下 3 个步骤:

1.在表中查找，如果找到，返回该元素，找不到则继续
2.判断该表是否有元表，如果没有元表，返回 nil，有元表则继续。
3.判断元表有没有 __index 方法，如果 __index 方法为 nil，则返回 nil；如果 __index 方法是一个表，则重复 1、2、3；如果 __index 方法是一个函数，则返回该函数的返回值。
该部分内容来自作者寰子：https://blog.csdn.net/xocoder/article/details/9028347
]]--
mytable = setmetatable({key1 = "Value1"},{
	__index = function(mytable,key1)
		if(key1 == "key2") then
			return "mymetatable"
		else
			return nil
		end
	end
})

print(mytable.key1,mytable.key2)--LUA: Value1	mymetatable

--__newindex 元方法
--实例中表设置了元方法 __newindex，在对新索引键（newkey）赋值时（mytable.newkey = "新值2"），会调用元方法，而不进行赋值。而如果对已存在的索引键（key1），则会进行赋值，而不调用元方法 __newindex。
mymetatable = {}
mytable = setmetatable({key1 = "Value1"}, { __newindex = mymetatable })

print(mytable.key1)--LUA: Value1

mytable.newkey = "新值2"
print(mytable.newkey,mymetatable.newkey)--LUA: nil	新值2

mytable.key1 = "新值1"
print(mytable.key1,mymetatable.key1)--LUA: 新值1	nil

mytable = setmetatable({key1 = "value"},{
	__newindex = function(mytable, key, value)
		rawset(mytable,key,"\""..value.."\"")
	end
})

mytable.key1 = "new value"
mytable.key2 = 4

print(mytable.key1,mytable.key2)--LUA: new value	"4"

--计算(下标从1开始的（lua默认下标从1开始）)表的元素个数
function table_maxn(t)
	local mn = 0
	for k,v in pairs(t) do
		if(mn < k) then
			mn = k
		end
	end
	return mn
end


mytable2 = {1,2,3}
--print(table_maxn(mytable2))--3
mytable3 = {[0] = 0,[1] = 1}
--print(table_maxn(mytable3))--1

--__add	对应的运算符 '+'.
mytable = setmetatable({1,2,3},{
	__add = function(myTable,newtable)
		for i = 1,table_maxn(newtable) do
			table.insert(mytable,table_maxn(mytable) + 1,newtable[i])
		end
		return mytable
	end
})

secondtable = {4,5,6}

mytable = mytable + secondtable

for k,v in pairs(mytable) do
	print(k,v) -- 1，2，3，4，5，6
end

--__call 元方法

mytable = setmetatable({10}, {
	__call = function(mytable, newtable)
		sum = 0
		for i=1,table_maxn(mytable) do
			sum = sum + mytable[i]
		end

		for i=1,table_maxn(newtable) do
			sum = sum + newtable[i]
		end

		return sum
	end
})

newtable = {10,20,30}
print(mytable(newtable)) --70


--__tostring 元方法
--__tostring 元方法用于修改表的输出行为。以下实例我们自定义了表的输出内容：

mytable =setmetatable({1,2,3},{
	__tostring = function(mytable)
		sum = 0
		for i=1,table_maxn(mytable) do
			sum = sum + mytable[i]
		end
		return "表所有的元素和为L:"..sum
	end
})

print(mytable)