--Lua table(表)		

--初始化表
myTable = {}

myTable = {[1] = 1,[1] = 11}

print(myTable[1])--LUA: 11 会被覆盖


--myTable的类型
print(type(myTable))--LUA: table

--指定值
myTable[1] = "lua"

print(myTable[1])

--当我们为 table a 并设置元素，然后将 a 赋值给 b，则 a 与 b 都指向同一个内存。如果 a 设置为 nil ，则 b 同样能访问 table 的元素。如果没有指定的变量指向a，Lua的垃圾回收机制会清理相对应的内存。
myTable["wow"] = "修改前"
print("myTable 索引为1的元素是",myTable[1])
print("myTable 索引为wow的元素是" ,myTable["wow"])


alternateTable = myTable

print("alternateTable 索引为1的元素是",alternateTable[1])
print("alternateTable 索引为wow的元素是" ,alternateTable["wow"])

alternateTable["wow"] = "修改后"

print("myTable 索引为wow的元素是" ,myTable["wow"])
print("alternateTable 索引为wow的元素是" ,alternateTable["wow"])

--释放变量
alternateTable = nil

print("alternateTable 是",alternateTable)
-- 结果:alternateTable 是 nil


--被释放之后，无法再进行访问 报错
--print("alternateTable 索引为wow的元素是" ,alternateTable["wow"])

--myTable 仍然可以访问
print("myTable 索引为wow的元素是" ,myTable["wow"])

--移除引用
myTable = nil
print("myTable 是",myTable)

--Table 连接

fruits = {"banana","orange","apple"}
-- 返回 table 连接后的字符串
print("table 连接后的字符串:"..table.concat(fruits))

print("table 连接后的字符串:"..table.concat(fruits, ", "))

print("table 连接后的字符串:"..table.concat(fruits, ", ", 1, 2 ))

--插入和移除
fruits = {"banana","orange","apple"}
table.insert(fruits,"mango")
print(table.concat(fruits, ", "))--LUA: banana, orange, apple, mango

table.insert(fruits,2,"grapes")--插入2号元素，其他的顺移到后面
print(table.concat(fruits, ", "))--LUA: banana, grapes, orange, apple, mango

--返回table数组部分位于pos位置的元素. 其后的元素会被前移. pos参数可选, 默认为table长度, 即从最后一个元素删起。
table.remove(fruits)
print(fruits == nil)--LUA: false
print(table.concat(fruits, ", "))--LUA: banana, grapes, orange, apple

table.remove(fruits,3)--移除了三号元素
print(table.concat(fruits, ", "))--LUA: banana, grapes, apple


mytable1 = {1,2,3,["key1"] = "value"}
print(table.concat( mytable1, ", ")) --LUA: 1, 2, 3


mytable2 = {1,2,3,[4] = "value"}
print(table.concat( mytable2, ", ")) --LUA: 1, 2, 3, value 只能拼接连续整形

mytable3 = {1,2,3,[5] = "value"}
print(table.concat( mytable3, ", ")) --LUA: 1, 2, 3 只能拼接连续整形

--table.remove(mytable1,"key1")--bad argument #2 to 'remove' (number expected, got string) --只能用int类型
print(table.concat( mytable1, ", ")) 

table.remove(mytable1,4)--不报错，但是因为mytable1不存在4元素，所以也remove不了元素4
print(table.concat( mytable1, ", ")) 


fruits = {"banana","orange","apple","grapes"}

print("排序前:"..table.concat(fruits, ", "))--LUA: 排序前:banana, orange, apple, grapes
table.sort(fruits)
print("排序后:"..table.concat(fruits, ", "))--LUA: 排序后:apple, banana, grapes, orange

numtable = {[2] = 2, [0] = 0, [1] = 1}
print("排序前:"..table.concat(numtable, ", "))--LUA: 排序前:1, 2  --依次按照key排序拼接
table.sort(numtable)
print("排序后:"..table.concat(numtable, ", "))--LUA: 排序前:1, 2 --依次按照key排序拼接

numtable1 = {[2] = 2, [0] = 0, [1] = 1}
for i,v in ipairs(numtable1) do
	print(i,v) -- 1 2
end

--Table 最大值
function table_maxn(t)
	local mn = nil
	for k,v in pairs(t) do

		if(mn == nil) then
			mn = v
		end

		if mn < v then
			mn = v
		end
	end
	return mn
end

tb1 = {[1] = 2,[2] = 6,[3] = 34,[4] = 5}

print("tab1 最大值："..table_maxn(tb1))
print("tab1 长度："..#tb1)