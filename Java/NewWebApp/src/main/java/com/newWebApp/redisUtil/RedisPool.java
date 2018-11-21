package com.newWebApp.redisUtil;

import java.util.ResourceBundle;

import redis.clients.jedis.JedisPool;
import redis.clients.jedis.JedisPoolConfig;

public class RedisPool {
    private static JedisPool pool;

    private RedisPool(){
        ResourceBundle bundle =ResourceBundle.getBundle("redis");
        //bundle类似一个map
        if(bundle==null){
            throw new IllegalArgumentException("[redis.properties] is not find ");
        }
        JedisPoolConfig config = new JedisPoolConfig();
        config.setMaxTotal(Integer.valueOf(bundle.getString("redis.pool.maxActive")));
        config.setMaxIdle(Integer.valueOf(bundle.getString("redis.pool.maxIdle")));
        config.setMaxWaitMillis(Long.valueOf(bundle.getString("redis.pool.maxWait")));
        config.setTestOnBorrow(Boolean.valueOf(bundle.getString("redis.pool.testOnBorrow")));
        config.setTestOnReturn(Boolean.valueOf(bundle.getString("redis.pool.testOnReturn")));

        //创建连接池
        pool =new JedisPool(config,bundle.getString("redis.ip"),Integer.valueOf(bundle.getString("redis.port")));
    }

    public synchronized static JedisPool getPool() {
        if(pool==null){
            new RedisPool();
        }
        return pool;
    }
}
