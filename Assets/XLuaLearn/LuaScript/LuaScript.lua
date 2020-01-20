local _, LuaDebuggee = pcall(require, 'LuaDebuggee')
if LuaDebuggee and LuaDebuggee.StartDebug then
	if LuaDebuggee.StartDebug('127.0.0.1', 9826) then
		print('LuaPerfect: Successfully connected to debugger!')
	else
		print('LuaPerfect: Failed to connect debugger!')
	end
else
	print('LuaPerfect: Check documents at: https://luaperfect.net')
end

local  util = require 'util'
local test1 = require 'test1'
local x = 1
-- Hello Word
print('hello,this message from lua Script !')

xlua.hotfix(CS.XLuaLoad,'Play',function(self)
	
	--loop()
	--print(max(1,2))
	--LuaClassTest()
	--LuaOperator()
	--LuaIterator()
	--local n,m = LuaRetun()
	--print("n:",n,"m",m)

	--print("play")
	--print(test1 == nil)
		
	--local temp = nil
	--print(temp)



	--xlua set字典 
	--self._dict[1][1]="ssss"
	--xlua get字典
	
	tempTable = {} 
	tempTable[0] = "000"
	tempTable[1] = "111"
	tempTable[2] = "222"

	self._dict[1] = tempTable

	print("lua:"..self._dict[1][1])

end)

xlua.private_accessible(CS.XLuaLoad)
util.hotfix_ex(CS.XLuaLoad,'Play1',function(self)
	--self:Play1()
	--print("do lua 2  Play1  Play1  Play1")
end)


xlua.hotfix(CS.XLuaLoad,'Awake',function(self)
	print("lua:"..self._dict[1][1])
end)

--可以一次返回多个参数
function LuaRetun( ... )
	x = 10
	y = 100
	return  x,y
end

--迭代器
function LuaIterator( ... )

	--[[
	array = {"arr1","arr2"}
	--pairs
	for key , value in pairs(array) do
		print(key,value)
	end
	--ipairs
	for  key , value in ipairs(array) do
		print(key,value)
	end
	]]

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
end

function  loop()

	local  x = 1
	while(x < 5)
	do
		print('循环...')
		x = x + 1
	end

end


function max(num1,num2 )

	if(num1 > num2) then
		result = num1
	else
		result = num2
	end

	return result
end

--table.sort( tablename, sortfunction )


function  LuaClassTest( ... )
	--初始化表
	myTable = {}
	--myTable的类型
	print(type(myTable))

	--指定值
	myTable[1] = "lua"

	print(myTable[1])
	

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

end

function  LuaOperator( ... )
	print(10^2)
	print(4^3 * 16^4 * 16^3)
end










