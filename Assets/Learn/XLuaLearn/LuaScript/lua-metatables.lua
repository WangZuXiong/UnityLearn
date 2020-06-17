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
myTable = setmetatable({key1 = "Value1"},{
	__index = function(myTable,key1)
		if(key1 == "key2") then
			return "mymetatable"
		else
			return nil
		end
	end
})

print(myTable.key1,myTable.key2)
