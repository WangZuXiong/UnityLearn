--Lua迭代器


	
array = {"arr1","arr2"}
--pairs
for key , value in pairs(array) do
	print(key,value)
end
--ipairs
for  key , value in ipairs(array) do
	print(key,value)
end


myTable = {}
myTable[1] = "111"
myTable[2] = "222"
myTable[4] = "444"
--pairs
print(">>>>pairs:")
for k,v in pairs(myTable) do
	print(k,v)
end

--ipairs 遍历一组数组，如果使用ipairs，遇到非连续的整形key就会停止便利，
print(">>>>ipairs:")
for i,v in ipairs(myTable) do
	print(i,v)
end






local  x = 1
while(x < 5)
do
	print('循环...')
	x = x + 1
end

