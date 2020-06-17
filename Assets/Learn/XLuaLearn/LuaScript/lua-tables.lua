	--Lua table(表)

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