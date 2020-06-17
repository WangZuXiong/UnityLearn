--Lua 函数
--可以一次返回多个参数

local x = 1

function LuaRetun( ... )
	x = 10
	y = 100
	return  x,y
end


function max(num1,num2 )

	if(num1 > num2) then
		result = num1
	else
		result = num2
	end

	return result
end
