package com.newWebApp.redisUtil;

import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;
import org.apache.log4j.Logger;
import org.apache.log4j.LogManager;
import org.apache.ibatis.cache.Cache;

import redis.clients.jedis.Jedis;

public class RedisCache implements Cache{
    private static Logger logger = LogManager.getLogger(RedisCache.class);//日志管理
    private String id;
    private Jedis redisClient=createRedis();  //创建一个jedis连接
    private ReadWriteLock readWriteLock = new ReentrantReadWriteLock(); //读写锁

    public void setReadWriteLock(ReadWriteLock readWriteLock) {
        this.readWriteLock = readWriteLock;
    }

    public RedisCache(String id) {
        if(id==null){
            throw new IllegalArgumentException("Cache instance requires an ID");
        }
        logger.debug("create an cache instance with id"+id);
        this.id=id;
    }

    public String getId() {
        return this.id;
    }

    /**从连接池中取
     * @return
     */
    private  static Jedis createRedis() {
        //从连接池获取redis连接
        Jedis jedis =RedisPool.getPool().getResource();
        return jedis;
    }

    public void putObject(Object key, Object value) {
        byte[] keybyte=SerializableUtil.serialize(key);
        byte[]valuebyte=SerializableUtil.serialize(value);
        this.redisClient.set(keybyte, valuebyte);
    }

    public Object getObject(Object key) {
        //缓存穿透

        byte[] values=this.redisClient.get(SerializableUtil.serialize(key));
        //算法：计算一定时间内没有命中的键，存起来   key->value
        if(values==null){
            return null;
        }
        Object obj =SerializableUtil.unserizlize(values);
        return obj;
    }

    public Object removeObject(Object key) {
        byte[]keybyte=SerializableUtil.serialize(key);
        return this.redisClient.expire(keybyte, 0);
    }

    public void clear() {
        this.redisClient.flushDB();
    }

    public int getSize() {
        Long size = this.redisClient.dbSize();
        int s =Integer.valueOf(size+"");
        return s;
    }

    public ReadWriteLock getReadWriteLock() {
        return readWriteLock;
    }

}
